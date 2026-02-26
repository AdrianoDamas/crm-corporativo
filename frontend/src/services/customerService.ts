import { http } from '../lib/http';
import type { Customer, CreateCustomerPayload, UpdateCustomerPayload } from '../types/customer';

export const customerService = {
  list: () => http.get<Customer[]>('/customers'),
  create: (payload: CreateCustomerPayload) => http.post<Customer>('/customers', payload),
  update: (id: string, payload: UpdateCustomerPayload) => http.put<Customer>(`/customers/${id}`, payload),
  deactivate: (id: string) => http.delete<void>(`/customers/${id}`),
};
