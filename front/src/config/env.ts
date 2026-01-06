const defaultApiBaseUrl =
  import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:43779/'

export const env = {
  apiBaseUrl: defaultApiBaseUrl,
}
