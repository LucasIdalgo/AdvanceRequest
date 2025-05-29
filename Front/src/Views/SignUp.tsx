import { Container, Box, Typography, Stack, TextField, Button } from "@mui/material";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

import type { SignUpProps } from "../Props/SignUpProps";

const isValidEmail = (email: string) => {
  if (!email.includes('@')) return false;

  const [user, domain] = email.split('@');
  if (!user || !domain || !domain.includes('.') || domain.startsWith('.') || domain.endsWith('.')) {
    return false;
  }
  return true;
};

export const SignUp: React.FC<SignUpProps> = ({ onRegister }) => {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState({ name: '', email: '', password: '' });

  const navigate = useNavigate();

  const validate = () => {
    let valid = true;
    let nomeErr = '', emailErr = '', passwordErr = '';

    if (!name.trim()) {
      nomeErr = 'Nome é obrigatório';
      valid = false;
    }

    if (!email.trim()) {
      emailErr = 'Email é obrigatório';
      valid = false;
    } else if (!isValidEmail(email)) {
      emailErr = 'Email inválido';
      valid = false;
    }

    if (!password.trim()) {
      passwordErr = 'Senha é obrigatória';
      valid = false;
    }

    setErrors({ name: nomeErr, email: emailErr, password: passwordErr });
    return valid;
  };

  const handleRegister = async () => {
    if (validate()) {
      const success = await onRegister(name, email, password);
      if (success)
        navigate('/login');
    }
  };

  return (
    <Container maxWidth="xs">
      <Box mt={8}>
        <Typography variant="h5" gutterBottom textAlign={"center"}>Cadastrar</Typography>
        <Stack spacing={2}>
          <TextField
            label="Nome"
            value={name}
            onChange={(e) => setName(e.target.value)}
            error={!!errors.name}
            helperText={errors.name}
            fullWidth
          />
          <TextField
            label="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            error={!!errors.email}
            helperText={errors.email}
            fullWidth
          />
          <TextField
            label="Senha"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            error={!!errors.password}
            helperText={errors.password}
            fullWidth
          />
          <Button variant="contained" onClick={handleRegister}>Cadastrar</Button>
          <Button variant="outlined" onClick={() => navigate('/login')}>Voltar</Button>
        </Stack>
      </Box>
    </Container>
  )
}