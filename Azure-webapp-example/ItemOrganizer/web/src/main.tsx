import { PublicClientApplication } from '@azure/msal-browser'
import { MsalProvider } from '@azure/msal-react'
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { AuthenticatedApp } from './AuthenticatedApp'
import './styles.css'

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL as string | undefined
const clientId = import.meta.env.VITE_ENTRA_CLIENT_ID as string | undefined
const tenantId = import.meta.env.VITE_ENTRA_TENANT_ID as string | undefined
const apiScope = import.meta.env.VITE_API_SCOPE as string | undefined
const useDevelopmentAuthentication =
  import.meta.env.VITE_DEVELOPMENT_AUTHENTICATION === 'true'
const root = createRoot(document.getElementById('root')!)

if (!apiBaseUrl) {
  root.render(
    <main className="sign-in">
      <p className="eyebrow">CONFIGURATION REQUIRED</p>
      <h1>Connect the Item Organizer.</h1>
      <p>
        Set VITE_API_BASE_URL in web/.env.local.
      </p>
    </main>,
  )
} else if (useDevelopmentAuthentication) {
  void import('./api').then(({ HttpInventoryApi }) => {
    void import('./InventoryApp').then(({ InventoryApp }) => {
      root.render(
        <StrictMode>
          <InventoryApp
            api={new HttpInventoryApi(apiBaseUrl, async () => null)}
          />
        </StrictMode>,
      )
    })
  })
} else if (!clientId || !tenantId || !apiScope) {
  root.render(
    <main className="sign-in">
      <p className="eyebrow">CONFIGURATION REQUIRED</p>
      <h1>Connect Microsoft Entra ID.</h1>
      <p>
        Set VITE_ENTRA_CLIENT_ID, VITE_ENTRA_TENANT_ID, and VITE_API_SCOPE in
        web/.env.local.
      </p>
    </main>,
  )
} else {
  const instance = new PublicClientApplication({
    auth: {
      clientId,
      authority: `https://login.microsoftonline.com/${tenantId}`,
      redirectUri: window.location.origin,
    },
    cache: {
      cacheLocation: 'sessionStorage',
    },
  })

  void instance.initialize().then(() => {
    root.render(
      <StrictMode>
        <MsalProvider instance={instance}>
          <AuthenticatedApp apiBaseUrl={apiBaseUrl} scope={apiScope} />
        </MsalProvider>
      </StrictMode>,
    )
  })
}
