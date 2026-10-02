import { defineConfig, devices } from '@playwright/test'

const developmentEnvironment = {
  ASPNETCORE_ENVIRONMENT: 'Development',
  ASPNETCORE_URLS: 'http://0.0.0.0:5000',
  Authentication__AllowedTenantId: '10000000-0000-0000-0000-000000000001',
  DevelopmentAuthentication__Enabled: 'true',
  DevelopmentAuthentication__OwnerObjectId:
    '20000000-0000-0000-0000-000000000001',
  DevelopmentAuthentication__TenantId:
    '10000000-0000-0000-0000-000000000001',
}

export default defineConfig({
  testDir: './e2e',
  fullyParallel: false,
  workers: 1,
  reporter: [['list'], ['html', { open: 'never' }]],
  use: {
    baseURL: 'http://localhost:5173',
    screenshot: 'only-on-failure',
    trace: 'retain-on-failure',
  },
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],
  webServer: [
    {
      command: 'dotnet run --project ../src/ItemOrganizer.Api --no-restore',
      env: developmentEnvironment,
      reuseExistingServer: false,
      timeout: 120_000,
      url: 'http://localhost:5000/api/v1/health/ready',
    },
    {
      command: 'pnpm dev',
      env: {
        VITE_API_BASE_URL: 'http://localhost:5000',
        VITE_DEVELOPMENT_AUTHENTICATION: 'true',
      },
      reuseExistingServer: false,
      timeout: 120_000,
      url: 'http://localhost:5173',
    },
  ],
})
