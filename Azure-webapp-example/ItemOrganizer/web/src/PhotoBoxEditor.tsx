import { KeyboardEvent, PointerEvent, useId, useRef } from 'react'
import { BoundingBox } from './api'

type Props = {
  photoUrl: string
  name: string
  box: BoundingBox | null
  disabled: boolean
  onChange: (box: BoundingBox | null) => void
}

type Gesture = {
  pointerId: number
  mode: 'move' | 'resize'
  startX: number
  startY: number
  width: number
  height: number
  box: BoundingBox
}

const clamp = (value: number, min: number, max: number) =>
  Math.min(max, Math.max(min, value))

function adjustBox(
  box: BoundingBox,
  mode: Gesture['mode'],
  dx: number,
  dy: number,
): BoundingBox {
  return mode === 'move'
    ? {
        ...box,
        x: clamp(box.x + dx, 0, 1 - box.width),
        y: clamp(box.y + dy, 0, 1 - box.height),
      }
    : {
        ...box,
        width: clamp(box.width + dx, 0.01, 1 - box.x),
        height: clamp(box.height + dy, 0.01, 1 - box.y),
      }
}

export function PhotoBoxEditor({
  photoUrl,
  name,
  box,
  disabled,
  onChange,
}: Props) {
  const helpId = useId()
  const canvas = useRef<HTMLDivElement>(null)
  const gesture = useRef<Gesture | null>(null)

  function startGesture(event: PointerEvent, mode: Gesture['mode']) {
    if (disabled || !box || event.button !== 0 || !canvas.current) return
    const bounds = canvas.current.getBoundingClientRect()
    if (!bounds.width || !bounds.height) return
    event.preventDefault()
    canvas.current.setPointerCapture(event.pointerId)
    gesture.current = {
      pointerId: event.pointerId,
      mode,
      startX: event.clientX,
      startY: event.clientY,
      width: bounds.width,
      height: bounds.height,
      box,
    }
  }

  function moveGesture(event: PointerEvent) {
    const current = gesture.current
    if (!current || current.pointerId !== event.pointerId || disabled) return
    onChange(
      adjustBox(
        current.box,
        current.mode,
        (event.clientX - current.startX) / current.width,
        (event.clientY - current.startY) / current.height,
      ),
    )
  }

  function endGesture(event: PointerEvent) {
    if (gesture.current?.pointerId !== event.pointerId) return
    gesture.current = null
    if (canvas.current?.hasPointerCapture(event.pointerId)) {
      canvas.current.releasePointerCapture(event.pointerId)
    }
  }

  function handleKey(event: KeyboardEvent, mode: Gesture['mode']) {
    if (!box || disabled) return
    const step = event.shiftKey ? 0.1 : 0.01
    let dx = 0
    let dy = 0
    switch (event.key) {
      case 'ArrowLeft':
        dx = -step
        break
      case 'ArrowRight':
        dx = step
        break
      case 'ArrowUp':
        dy = -step
        break
      case 'ArrowDown':
        dy = step
        break
      default:
        return
    }
    event.preventDefault()
    onChange(adjustBox(box, mode, dx, dy))
  }

  return (
    <fieldset className="photo-box-editor" disabled={disabled}>
      <legend>Item crop for {name}</legend>
      <div className="photo-preview">
        <div
          className="photo-canvas"
          ref={canvas}
          onPointerMove={moveGesture}
          onPointerUp={(event) => {
            moveGesture(event)
            endGesture(event)
          }}
          onPointerCancel={endGesture}
          onLostPointerCapture={() => {
            gesture.current = null
          }}
        >
          <img src={photoUrl} alt={`Source photo for ${name}`} draggable={false} />
          {box && (
            <>
              <button
                type="button"
                className="bounding-box editable-box"
                aria-label={`Move crop for ${name}`}
                aria-describedby={helpId}
                style={{
                  left: `${box.x * 100}%`,
                  top: `${box.y * 100}%`,
                  width: `${box.width * 100}%`,
                  height: `${box.height * 100}%`,
                }}
                onPointerDown={(event) => startGesture(event, 'move')}
                onKeyDown={(event) => handleKey(event, 'move')}
              >
                <span className="box-label">{name}</span>
              </button>
              <button
                type="button"
                className="box-resize-handle"
                aria-label={`Resize crop for ${name}`}
                aria-describedby={helpId}
                style={{
                  left: `${(box.x + box.width) * 100}%`,
                  top: `${(box.y + box.height) * 100}%`,
                }}
                onPointerDown={(event) => startGesture(event, 'resize')}
                onKeyDown={(event) => handleKey(event, 'resize')}
              />
            </>
          )}
        </div>
      </div>
      <p className="field-help" id={helpId}>
        {box
          ? 'Drag the box to move it; drag its corner to resize. Or focus the box or corner and use arrow keys.'
          : 'No crop selected. Add a box to mark the item on the photo.'}
      </p>
      <button
        type="button"
        className="secondary"
        onClick={() =>
          onChange(box ? null : {
            x: 0.1,
            y: 0.1,
            width: 0.25,
            height: 0.25,
          })
        }
      >
        {box ? 'Remove crop' : 'Add crop'}
      </button>
    </fieldset>
  )
}
