---
name: get-weather
description: Fetches the current weather or forecast for a given city or location. Use this skill when the user asks about weather, temperature, forecast, rain, wind, humidity, or conditions in any place.
argument-hint: city or location
---

# Get Weather Skill

## When to Use
Use this skill whenever the user asks about:
- Current weather conditions in a city or location
- Temperature (today, this week)
- Forecast (hourly, daily, weekly)
- Rain, snow, wind, humidity, or UV index
- Phrases like "what's the weather in X", "is it raining in X", "will it snow"

## How to Fetch Weather

Use [wttr.in](https://wttr.in) — no API key required.

### PowerShell (Windows)
```powershell
# Current weather (plain text, one-line)
(Invoke-WebRequest -Uri "https://wttr.in/Tokyo?format=3" -UseBasicParsing).Content

# Full forecast (3 days)
(Invoke-WebRequest -Uri "https://wttr.in/Tokyo?format=4" -UseBasicParsing).Content

# JSON for programmatic use
$weather = Invoke-WebRequest -Uri "https://wttr.in/Tokyo?format=j1" -UseBasicParsing | ConvertFrom-Json
$weather.current_condition[0]
```

### Shell / curl (macOS/Linux)
```bash
# One-line summary
curl "https://wttr.in/Tokyo?format=3"

# Full forecast
curl "https://wttr.in/Tokyo"

# JSON
curl "https://wttr.in/Tokyo?format=j1" | jq '.current_condition[0]'
```

### Python
```python
import urllib.request, json

city = "Tokyo"
url = f"https://wttr.in/{city}?format=j1"
with urllib.request.urlopen(url) as r:
    data = json.loads(r.read())

current = data["current_condition"][0]
print(f"Temp: {current['temp_C']}°C / {current['temp_F']}°F")
print(f"Feels like: {current['FeelsLikeC']}°C")
print(f"Humidity: {current['humidity']}%")
print(f"Wind: {current['windspeedKmph']} km/h {current['winddir16Point']}")
print(f"Description: {current['weatherDesc'][0]['value']}")
```

## Format Codes (wttr.in)
| Format | Description |
|--------|-------------|
| `?format=3` | `City: ⛅ +18°C` one-liner |
| `?format=4` | Multi-line with forecast |
| `?format=j1` | Full JSON response |
| `?format=%t` | Temperature only |
| `?format=%h` | Humidity only |
| `?format=%w` | Wind only |
| `?format=%C` | Condition description |

## Steps
1. Extract the city/location from the user's request
2. Run the appropriate command for the current OS
3. Parse and present the result in a readable format
4. If the city name has spaces, replace them with `+` (e.g., `New+York`)

## Example Interactions
- "What's the weather in London?" → `curl "https://wttr.in/London?format=3"`
- "Is it going to rain in Seattle this week?" → fetch JSON and check `weather[].hourly[].chanceofrain`
- "Temperature in Paris" → `curl "https://wttr.in/Paris?format=%t"`
