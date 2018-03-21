import requests
import json

# Read credentials.
with open('ups_creds.json') as creds:
  data = json.load(creds)
  user_name = data['user_name']
  password = data['password']
  access_license_number = data['access_license_number']

# Need replace this number with a real tracking number.
track_number = "1Z12345E1512345676"

# Send REST request.
ups_tracking_test_url = 'https://wwwcie.ups.com/rest/Track'
ups_tracking_url = 'https://onlinetools.ups.com/rest/Track'

track_request_json = {
  "UPSSecurity": {
    "UsernameToken": {
      "Username": user_name,
      "Password": password
    },
    "ServiceAccessToken": {
      "AccessLicenseNumber": access_license_number,
    }
  },
  "TrackRequest": {
    "Request": {
      "RequestOption": "1",
      "TransactionReference": {
        "CustomerContext": "Get tracking status"
      }
    },
    "InquiryNumber": track_number
  }
}

response = requests.post(ups_tracking_url, data=json.dumps(track_request_json))

print(response.json())