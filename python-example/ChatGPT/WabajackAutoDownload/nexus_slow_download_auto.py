
"""
NexusMods "Slow download" auto-clicker
--------------------------------------
Every 5 seconds, scan the screen for the NexusMods download page.
If found, scroll down to locate the "Slow download" button and click it.

Requirements (install on your machine):
  pip install pyautogui opencv-python numpy pillow

Optional but recommended:
  - Turn OFF browser zoom (set to 100%) for best matching.
  - Keep Windows display scaling at 100% if possible.
  - Run this script on the same monitor where the browser is visible.

Run:
  python nexus_slow_download_auto.py

Stop:
  Press Ctrl+C in the terminal window.
"""

import time
import sys
import threading
from pathlib import Path

import numpy as np

try:
    import cv2
except Exception as e:
    print("OpenCV (cv2) is required: pip install opencv-python")
    raise

import pyautogui

# Fail-safe: moving mouse to a screen corner aborts pyautogui actions
pyautogui.FAILSAFE = True
pyautogui.PAUSE = 0.05

HERE = Path(__file__).resolve().parent
TEMPLATE_SLOW = HERE / "slow_download.png"
TEMPLATE_TITLE = HERE / "choose_download_type.png"

if not TEMPLATE_SLOW.exists():
    print(f"[ERROR] Missing template image: {TEMPLATE_SLOW}")
    sys.exit(1)
if not TEMPLATE_TITLE.exists():
    print(f"[WARN] Missing page detector template: {TEMPLATE_TITLE} (continuing anyway)")


def screenshot_bgr():
    """Take a screenshot and return as a BGR numpy array for OpenCV."""
    shot = pyautogui.screenshot("screen.png")
    img = cv2.cvtColor(np.array(shot), cv2.COLOR_RGB2BGR)
    return img


def match_template_multi_scale(haystack_bgr, template_bgr, scales=(0.8, 0.9, 1.0, 1.1, 1.2), method=cv2.TM_CCOEFF_NORMED, threshold=0.85):
    """
    Try template matching across multiple scales.
    Returns best location (x, y, w, h, score) or None if not found.
    """
    h, w = None, None
    best = None
    for s in scales:
        th, tw = int(template_bgr.shape[0] * s), int(template_bgr.shape[1] * s)
        if th < 10 or tw < 10:
            continue
        resized = cv2.resize(template_bgr, (tw, th), interpolation=cv2.INTER_AREA)
        res = cv2.matchTemplate(haystack_bgr, resized, method)
        min_val, max_val, min_loc, max_loc = cv2.minMaxLoc(res)

        score = max_val if method in [cv2.TM_CCOEFF, cv2.TM_CCOEFF_NORMED] else 1 - min_val
        loc = max_loc if method in [cv2.TM_CCOEFF, cv2.TM_CCOEFF_NORMED] else min_loc

        if best is None or score > best[-1]:
            best = (loc[0], loc[1], tw, th, score)

    if best and best[-1] >= threshold:
        return best
    return None


def scroll_page_down(steps=3, amount=-800):
    """Scroll down the active window a few times (negative amount scrolls down)."""
    for _ in range(steps):
        pyautogui.scroll(amount)
        time.sleep(0.25)


def center_of_bbox(bbox):
    x, y, w, h = bbox[:4]
    return (x + w // 2, y + h // 2)


def find_and_click_slow_button():
    """Try to find and click the 'Slow download' button. Returns True if clicked."""
    print(f"[INFO] Taking screenshot...")
    hay = screenshot_bgr()

    # Load templates
    slow = cv2.imread(str(TEMPLATE_SLOW))
    if slow is None:
        print("[ERROR] Failed to load slow_download.png")
        return False
    title = None
    if TEMPLATE_TITLE.exists():
        title = cv2.imread(str(TEMPLATE_TITLE))

    # Optional: check if we're on the download page
    print(f"[INFO] Checking if on download page...")
    on_page = True
    if title is not None:
        page_hit = match_template_multi_scale(hay, title, threshold=0.80)
        on_page = page_hit is not None

    if not on_page:
        print(f"[WARN] Not on download page.")
        return False

    # Try to locate button directly. If not found, scroll and try again.
    print(f"[INFO] Finding 'Slow download' button...")
    for attempt in range(2):  # up to ~6 scrolls
        hay = screenshot_bgr()
        hit = match_template_multi_scale(hay, slow, threshold=0.87)
        if hit:
            cx, cy = center_of_bbox(hit)
            print(f"[INFO] 'Slow download' button found at ({cx},{cy})")

            # Move and click
            pyautogui.moveTo(cx, cy, duration=0.15)
            pyautogui.click()
            print(f"[OK] Clicked 'Slow download' at ({cx},{cy})")
            return True

        # if not found, scroll a bit and try again
        print(f"[INFO] 'Slow download' button not found, scrolling...(attempt {attempt})")
        scroll_page_down(steps=1, amount=-50)

    print(f"[WARN] 'Slow download' button not found after scrolling.")
    return False

def main():
    print("NexusMods 'Slow download' auto-clicker started.")
    print("Every 5 seconds I'll scan for the page and click 'Slow download' if visible.")
    print("Press Ctrl+C to stop.\n")

    while True:
        try:
            clicked = find_and_click_slow_button()
            time.sleep(5.0)
        except KeyboardInterrupt:
            print("\nStopped by user.")
            break
        except Exception as e:
            print(f"[WARN] {e.__class__.__name__}: {e}")
            time.sleep(5.0)


if __name__ == "__main__":
    main()
