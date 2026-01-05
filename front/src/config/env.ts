const defaultApiBaseUrl =
  import.meta.env.VITE_API_BASE_URL ?? 'https://localhost:44365/'

export const env = {
  apiBaseUrl: defaultApiBaseUrl,
}
