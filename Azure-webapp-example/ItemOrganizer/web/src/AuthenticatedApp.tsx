import { useIsAuthenticated, useMsal } from '@azure/msal-react'
import { useMemo } from 'react'
import { HttpInventoryApi } from './api'
import { InventoryApp } from './InventoryApp'

export function AuthenticatedApp({
  apiBaseUrl,
  scope,
}: {
  apiBaseUrl: string
  scope: string
}) {
  const { instance, accounts } = useMsal()
  const isAuthenticated = useIsAuthenticated()
  const api = useMemo(
    () =>
      new HttpInventoryApi(apiBaseUrl, async () => {
        const account = accounts[0]
        if (!account) {
          throw new Error('Sign in before accessing inventory.')
        }
        const result = await instance.acquireTokenSilent({
          account,
          scopes: [scope],
        })
        return result.accessToken
      }),
    [accounts, apiBaseUrl, instance, scope],
  )

  if (!isAuthenticated) {
    return (
      <main className="sign-in">
        <p className="eyebrow">ITEM ORGANIZER</p>
        <h1>Your inventory, clearly organized.</h1>
        <p>Sign in with your Microsoft Entra account to continue.</p>
        <button
          onClick={() => void instance.loginPopup({ scopes: [scope] })}
        >
          Sign in
        </button>
      </main>
    )
  }

  return <InventoryApp api={api} />
}
