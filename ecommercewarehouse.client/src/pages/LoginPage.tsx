import { useState, type FormEvent } from 'react';
import { setToken } from '../auth.ts';
import { apiFetch } from '../auth.ts';

export default function LoginPage({ onLogin }: { onLogin: () => void }) {
  const [email, setEmail] = useState('admin@ew.local');
  const [password, setPassword] = useState('123456');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const res = await apiFetch('/api/auth/login', {
        method: 'POST',
        body: JSON.stringify({ email, password })
      });
      if (!res.ok) {
        let message = 'Login failed';
        try { const data = await res.json(); message = data.error || message; } catch {}
        setError(message);
      } else {
        const data = await res.json();
        setToken(data.token);
        onLogin();
      }
    } catch (err) {
      setError('Network error');
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="login-page">
      <div className="login-card">
        <h2>Welcome</h2>
        <p className="muted">Please sign in to continue</p>
        {error && <div className="error">{error}</div>}
        <form onSubmit={handleSubmit} className="login-form">
          <label>Email</label>
          <input value={email} onChange={e => setEmail(e.target.value)} />
          <label>Password</label>
          <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
          <button type="submit" disabled={loading}>{loading ? '...' : 'Login'}</button>
        </form>
      </div>
    </div>
  );
}
