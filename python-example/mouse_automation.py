import pyautogui
import time
import sys

# Set a fail-safe to prevent the mouse from getting stuck
pyautogui.FAILSAFE = True

def scroll_and_click(scroll_amount=100, interval=1.0, clicks=1):
    """
    Performs automated mouse scrolling and clicking
    
    Args:
        scroll_amount (int): Amount to scroll (positive for down, negative for up)
        interval (float): Time between actions in seconds
        clicks (int): Number of cycles to perform
    """
    try:
        # Give user time to switch to target window
        print("Starting in 5 seconds... Move mouse to top-left corner to stop.")
        time.sleep(5)
        
        for i in range(clicks):
            # Scroll
            pyautogui.scroll(scroll_amount)
            print(f"Scrolled {scroll_amount} units")
            
            # Get current mouse position
            x, y = pyautogui.position()
            print(f"Mouse position: ({x}, {y})")
            
            # Click at current position
            pyautogui.click()
            print(f"Clicked at position ({x}, {y})")
            
            # Wait for specified interval
            time.sleep(interval)
            
    except KeyboardInterrupt:
        print("\nStopped by user")
    except Exception as e:
        print(f"An error occurred: {e}")

if __name__ == "__main__":
    if len(sys.argv) > 1:
        scroll_amt = int(sys.argv[1])
        interval = float(sys.argv[2]) if len(sys.argv) > 2 else 1.0
        num_clicks = int(sys.argv[3]) if len(sys.argv) > 3 else 1
        scroll_and_click(scroll_amt, interval, num_clicks)
    else:
        print("Usage: python mouse_automation.py <scroll_amount> [interval] [clicks]")
        print("Example: python mouse_automation.py 100 1.5 5")
        print("This will scroll down 100 units, wait 1.5 seconds, and click, repeated 5 times")
