export const API_BASE = (import.meta.env.VITE_API_BASE as string) || '';

export function setToken(token: string) {
  localStorage.setItem('jwt', token);
}

export function getToken(): string | null {
  return localStorage.getItem('jwt');
}

export function clearToken() {
  localStorage.removeItem('jwt');
}

export async function apiFetch(input: RequestInfo | URL, init: RequestInit = {}) {
  const token = getToken();
  const headers = new Headers(init.headers || {});
  headers.set('Content-Type', 'application/json');
  if (token) headers.set('Authorization', `Bearer ${token}`);

  let url: RequestInfo | URL = input;
  if (typeof input === 'string' && API_BASE) {
    const base = API_BASE.endsWith('/') ? API_BASE.slice(0, -1) : API_BASE;
    const path = input.startsWith('/') ? input : `/${input}`;
    url = `${base}${path}`;
  }

  return fetch(url, { ...init, headers });
}
