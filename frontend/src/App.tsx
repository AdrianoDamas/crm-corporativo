import { useCallback, useEffect, useState } from 'react';
import './App.css';
import { CustomerForm } from './components/CustomerForm';
import { CustomerList } from './components/CustomerList';
import { customerService } from './services/customerService';
import type { CreateCustomerPayload, Customer } from './types/customer';

function App() {
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [isLoading, setLoading] = useState(true);
  const [isSubmitting, setSubmitting] = useState(false);
  const [message, setMessage] = useState<string | null>(null);

  const loadCustomers = useCallback(async () => {
    try {
      setLoading(true);
      const data = await customerService.list();
      setCustomers(data);
    } catch (error) {
      setMessage(error instanceof Error ? error.message : 'Falha ao carregar clientes');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadCustomers();
  }, [loadCustomers]);

  async function handleCreate(payload: CreateCustomerPayload) {
    try {
      setSubmitting(true);
      setMessage(null);
      await customerService.create(payload);
      await loadCustomers();
    } catch (error) {
      setMessage(error instanceof Error ? error.message : 'Erro ao salvar cliente');
    } finally {
      setSubmitting(false);
    }
  }

  async function handleDeactivate(id: string) {
    try {
      setMessage(null);
      await customerService.deactivate(id);
      await loadCustomers();
    } catch (error) {
      setMessage(error instanceof Error ? error.message : 'Erro ao desativar cliente');
    }
  }

  return (
    <div className="layout">
      <header>
        <div>
          <p className="eyebrow">CRM Corporativo</p>
          <h1>Clientes</h1>
        </div>
        <p className="subtle">API em {import.meta.env.VITE_API_BASE_URL}</p>
      </header>

      {message && (
        <div className="alert">
          <p>{message}</p>
        </div>
      )}

      <section className="grid-layout">
        <CustomerForm onSubmit={handleCreate} isSubmitting={isSubmitting} />
        <CustomerList customers={customers} isLoading={isLoading} onDeactivate={handleDeactivate} />
      </section>
    </div>
  );
}

export default App;
