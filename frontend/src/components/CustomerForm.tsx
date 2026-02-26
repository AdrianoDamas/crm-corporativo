import { FormEvent, useState } from 'react';
import type { CreateCustomerPayload } from '../types/customer';
import { CustomerType, DocumentType } from '../types/customer';

interface CustomerFormProps {
  onSubmit: (payload: CreateCustomerPayload) => Promise<void>;
  isSubmitting: boolean;
}

const emptyAddress = () => ({
  zipCode: '',
  street: '',
  number: '',
  neighborhood: '',
  city: '',
  state: '',
  complement: '',
});

const buildDefaultState = (): CreateCustomerPayload => ({
  name: '',
  type: CustomerType.Individual,
  documentType: DocumentType.Cpf,
  document: '',
  email: '',
  phone: '',
  birthDate: '',
  stateRegistration: '',
  isStateRegistrationExempt: true,
  address: emptyAddress(),
});

export function CustomerForm({ onSubmit, isSubmitting }: CustomerFormProps) {
  const [form, setForm] = useState<CreateCustomerPayload>(buildDefaultState);
  const isIndividual = form.type === CustomerType.Individual;

  function updateField<T extends keyof CreateCustomerPayload>(field: T, value: CreateCustomerPayload[T]) {
    setForm((prev) => ({
      ...prev,
      [field]: value,
      ...(field === 'type'
        ? {
            documentType: value === CustomerType.Individual ? DocumentType.Cpf : DocumentType.Cnpj,
            isStateRegistrationExempt: value === CustomerType.Individual ? true : prev.isStateRegistrationExempt,
            stateRegistration: value === CustomerType.Individual ? '' : prev.stateRegistration,
            birthDate: value === CustomerType.Company ? '' : prev.birthDate,
          }
        : null),
    }));
  }

  function updateAddress(field: keyof typeof emptyAddress, value: string) {
    setForm((prev) => ({
      ...prev,
      address: {
        ...(prev.address ?? emptyAddress()),
        [field]: value,
      },
    }));
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const payload: CreateCustomerPayload = {
      ...form,
      document: form.document.trim(),
      email: form.email.trim(),
      phone: form.phone?.trim() || undefined,
      birthDate: form.birthDate ? new Date(form.birthDate).toISOString() : undefined,
      stateRegistration: form.type === CustomerType.Company ? form.stateRegistration?.trim() || undefined : undefined,
      isStateRegistrationExempt: form.type === CustomerType.Individual ? true : form.isStateRegistrationExempt,
      address: form.address?.street ? { ...form.address } : undefined,
    };

    await onSubmit(payload);
    setForm(buildDefaultState());
  }

  return (
    <form className="panel" onSubmit={handleSubmit}>
      <div className="panel-header">
        <div>
          <p className="eyebrow">Cadastro</p>
          <h2>Novo cliente</h2>
        </div>
        <button type="submit" disabled={isSubmitting}>
          {isSubmitting ? 'Salvando...' : 'Salvar cliente'}
        </button>
      </div>

      <div className="grid two-columns">
        <label>
          <span>Nome / Razão Social</span>
          <input
            required
            value={form.name}
            onChange={(event) => updateField('name', event.target.value)}
          />
        </label>

        <label>
          <span>E-mail</span>
          <input
            required
            type="email"
            value={form.email}
            onChange={(event) => updateField('email', event.target.value)}
          />
        </label>

        <label>
          <span>Telefone</span>
          <input
            value={form.phone}
            onChange={(event) => updateField('phone', event.target.value)}
          />
        </label>

        <label>
          <span>Tipo de cliente</span>
          <select
            value={form.type}
            onChange={(event) => updateField('type', Number(event.target.value) as CustomerType)}
          >
            <option value={CustomerType.Individual}>Pessoa Física</option>
            <option value={CustomerType.Company}>Pessoa Jurídica</option>
          </select>
        </label>

        <label>
          <span>{isIndividual ? 'CPF' : 'CNPJ'}</span>
          <input
            required
            value={form.document}
            onChange={(event) => updateField('document', event.target.value)}
          />
        </label>

        {isIndividual ? (
          <label>
            <span>Data de nascimento</span>
            <input
              required
              type="date"
              value={form.birthDate ?? ''}
              onChange={(event) => updateField('birthDate', event.target.value)}
            />
          </label>
        ) : (
          <>
            <label>
              <span>Inscrição estadual</span>
              <input
                value={form.stateRegistration}
                onChange={(event) => updateField('stateRegistration', event.target.value)}
                disabled={form.isStateRegistrationExempt}
                required={!form.isStateRegistrationExempt}
              />
            </label>

            <label className="checkbox">
              <input
                type="checkbox"
                checked={form.isStateRegistrationExempt}
                onChange={(event) => updateField('isStateRegistrationExempt', event.target.checked)}
              />
              <span>Isento</span>
            </label>
          </>
        )}
      </div>

      <div className="panel-divider" />
      <p className="eyebrow">Endereço</p>

      <div className="grid three-columns">
        <label>
          <span>CEP</span>
          <input
            value={form.address?.zipCode ?? ''}
            onChange={(event) => updateAddress('zipCode', event.target.value)}
          />
        </label>
        <label>
          <span>Rua</span>
          <input
            value={form.address?.street ?? ''}
            onChange={(event) => updateAddress('street', event.target.value)}
          />
        </label>
        <label>
          <span>Número</span>
          <input
            value={form.address?.number ?? ''}
            onChange={(event) => updateAddress('number', event.target.value)}
          />
        </label>
        <label>
          <span>Bairro</span>
          <input
            value={form.address?.neighborhood ?? ''}
            onChange={(event) => updateAddress('neighborhood', event.target.value)}
          />
        </label>
        <label>
          <span>Cidade</span>
          <input
            value={form.address?.city ?? ''}
            onChange={(event) => updateAddress('city', event.target.value)}
          />
        </label>
        <label>
          <span>Estado</span>
          <input
            value={form.address?.state ?? ''}
            onChange={(event) => updateAddress('state', event.target.value)}
          />
        </label>
        <label className="full-width">
          <span>Complemento</span>
          <input
            value={form.address?.complement ?? ''}
            onChange={(event) => updateAddress('complement', event.target.value)}
          />
        </label>
      </div>
    </form>
  );
}
