import type { Customer } from '../types/customer';
import { CustomerType } from '../types/customer';

interface CustomerListProps {
  customers: Customer[];
  isLoading: boolean;
  onDeactivate: (id: string) => Promise<void>;
}

export function CustomerList({ customers, isLoading, onDeactivate }: CustomerListProps) {
  if (isLoading) {
    return (
      <div className="panel">
        <p>Carregando clientes...</p>
      </div>
    );
  }

  if (!customers.length) {
    return (
      <div className="panel empty-state">
        <p>Cadastre o primeiro cliente para começar a usar o CRM.</p>
      </div>
    );
  }

  return (
    <div className="panel">
      <div className="panel-header">
        <div>
          <p className="eyebrow">Clientes</p>
          <h2>Base ativa</h2>
        </div>
      </div>

      <div className="table-wrapper">
        <table>
          <thead>
            <tr>
              <th>Nome</th>
              <th>Documento</th>
              <th>E-mail</th>
              <th>Status</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {customers.map((customer) => (
              <tr key={customer.id}>
                <td>
                  <strong>{customer.name}</strong>
                  <span className="subtle">{customer.type === CustomerType.Individual ? 'Pessoa Física' : 'Pessoa Jurídica'}</span>
                </td>
                <td>{customer.document}</td>
                <td>{customer.email}</td>
                <td>
                  <span className={`badge ${customer.isActive ? 'success' : 'muted'}`}>
                    {customer.isActive ? 'Ativo' : 'Inativo'}
                  </span>
                </td>
                <td>
                  {customer.isActive && (
                    <button type="button" className="ghost" onClick={() => onDeactivate(customer.id)}>
                      Desativar
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
