import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7293/api'
});

api.interceptors.request.use(config => {
  const token = localStorage.getItem('token')?.trim();
  if (token)
    config.headers.Authorization = `Bearer ${token}`;
  
  return config;
});

export default api;
