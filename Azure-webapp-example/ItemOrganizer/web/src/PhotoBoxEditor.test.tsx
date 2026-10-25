import { useState } from 'react'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { expect, test, vi } from 'vitest'
import { BoundingBox } from './api'
import { PhotoBoxEditor } from './PhotoBoxEditor'

const initialBox = { x: 0.1, y: 0.2, width: 0.4, height: 0.3 }

function Editor({ box = initialBox }: { box?: BoundingBox | null }) {
  const [value, setValue] = useState(box)
  return (
    <PhotoBoxEditor
      photoUrl="blob:photo"
      name="Tape"
      box={value}
      disabled={false}
      onChange={setValue}
    />
  )
}

test('shows the box on the source photo without numeric crop fields', () => {
  render(<Editor />)
  expect(screen.getByAltText('Source photo for Tape')).toHaveAttribute('src', 'blob:photo')
  expect(screen.getByRole('button', { name: 'Move crop for Tape' })).toHaveStyle({
    left: '10%', top: '20%', width: '40%', height: '30%',
  })
  expect(screen.queryByRole('spinbutton')).not.toBeInTheDocument()
})

test('moves and resizes with the keyboard, keeping the box inside the photo', async () => {
  const user = userEvent.setup()
  render(<Editor />)
  const box = screen.getByRole('button', { name: 'Move crop for Tape' })
  box.focus()
  await user.keyboard('{Shift>}{ArrowRight}{ArrowDown}{/Shift}')
  expect(box).toHaveStyle({ left: '20%' })
  await user.keyboard('{ArrowLeft>40/}{ArrowUp>40/}')
  expect(box).toHaveStyle({ left: '0%', top: '0%' })

  screen.getByRole('button', { name: 'Resize crop for Tape' }).focus()
  await user.keyboard('{Shift>}{ArrowRight>20/}{ArrowDown>20/}{/Shift}')
  expect(box).toHaveStyle({ width: '100%', height: '100%' })
  box.focus()
  await user.keyboard('{ArrowRight}{ArrowDown}')
  expect(box).toHaveStyle({ left: '0%', top: '0%' })
  screen.getByRole('button', { name: 'Resize crop for Tape' }).focus()
  await user.keyboard('{Shift>}{ArrowLeft>20/}{ArrowUp>20/}{/Shift}')
  expect(box).toHaveStyle({ width: '1%', height: '1%' })
})

test('adds and removes a box visually', async () => {
  const user = userEvent.setup()
  render(<Editor box={null} />)
  expect(screen.queryByRole('button', { name: 'Move crop for Tape' })).not.toBeInTheDocument()
  await user.click(screen.getByRole('button', { name: 'Add crop' }))
  expect(screen.getByRole('button', { name: 'Move crop for Tape' })).toBeVisible()
  await user.click(screen.getByRole('button', { name: 'Remove crop' }))
  expect(screen.queryByRole('button', { name: 'Move crop for Tape' })).not.toBeInTheDocument()
})

test('prevents changes for rejected or already confirmed detections', async () => {
  const user = userEvent.setup()
  const onChange = vi.fn()
  render(
    <PhotoBoxEditor
      photoUrl="blob:photo"
      name="Tape"
      box={initialBox}
      disabled
      onChange={onChange}
    />,
  )
  const box = screen.getByRole('button', { name: 'Move crop for Tape' })
  expect(box).toBeDisabled()
  expect(screen.getByRole('button', { name: 'Resize crop for Tape' })).toBeDisabled()
  await user.click(screen.getByRole('button', { name: 'Remove crop' }))
  expect(onChange).not.toHaveBeenCalled()
})
