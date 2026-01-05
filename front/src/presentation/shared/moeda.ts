export const formatarMoeda = (value?: number) =>
  value == null ? '-' : value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })