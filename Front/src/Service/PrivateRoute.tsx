import React from 'react';
import { Navigate } from 'react-router-dom';

import type { PrivateRouteProps } from '../Hooks/PrivateRouteProps';

export const PrivateRoute: React.FC<PrivateRouteProps> = ({ children }) => {
  const token = localStorage.getItem('token');
  return token ? children : <Navigate to="/login" />;
};