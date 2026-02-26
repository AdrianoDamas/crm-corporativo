export enum CustomerType {
  Individual = 0,
  Company = 1,
}

export enum DocumentType {
  Cpf = 1,
  Cnpj = 2,
}

export interface Address {
  zipCode: string;
  street: string;
  number: string;
  neighborhood: string;
  city: string;
  state: string;
  complement?: string | null;
}

export interface Customer {
  id: string;
  name: string;
  type: CustomerType;
  document: string;
  documentType: DocumentType;
  email: string;
  phone?: string | null;
  address?: Address | null;
  birthDate?: string | null;
  isActive: boolean;
  stateRegistration?: string | null;
  isStateRegistrationExempt: boolean;
  createdAt: string;
  updatedAt?: string | null;
}

export interface CreateCustomerPayload {
  name: string;
  type: CustomerType;
  documentType: DocumentType;
  document: string;
  email: string;
  phone?: string;
  birthDate?: string;
  stateRegistration?: string;
  isStateRegistrationExempt: boolean;
  address?: Address;
}

export interface UpdateCustomerPayload {
  name: string;
  email: string;
  phone?: string;
  birthDate?: string;
  stateRegistration?: string;
  isStateRegistrationExempt?: boolean;
  address?: Address;
}
