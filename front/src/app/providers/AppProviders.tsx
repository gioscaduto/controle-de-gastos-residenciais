import type { PropsWithChildren } from 'react'
import { ConfigProvider, App as AntdApp } from 'antd'
import ptBR from 'antd/locale/pt_BR'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 30_000,
      refetchOnWindowFocus: false,
    },
  },
})

export function AppProviders({ children }: PropsWithChildren) {
  return (
    <ConfigProvider
      locale={ptBR}
      theme={{
        token: {
          fontFamily: '"Segoe UI", Arial, sans-serif',
          colorPrimary: '#1677ff',
        },
      }}
    >
      <QueryClientProvider client={queryClient}>
        <AntdApp>{children}</AntdApp>
      </QueryClientProvider>
    </ConfigProvider>
  )
}
