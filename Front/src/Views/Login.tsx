import { useState } from "react";
import { Box, Button, Container, Stack, TextField, Typography } from "@mui/material"
import { useNavigate } from "react-router-dom";

import type { LoginProps } from "../Props/LoginProps";

const isValidEmail = (email: String) => {
  if (!email.includes("@")) return false;

  const [user, domain] = email.split("@");
  if (!user || !domain || !domain.includes(".") || domain.startsWith(".") || domain.endsWith(".")) return false;

  return true;
}

export const Login: React.FC<LoginProps> = ({ onLogin }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState({ email: '', password: '' });
  const navigate = useNavigate();

  const validate = () => {
    let valid = true;
    let emailErr = '', passwordErr = '';

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

    setErrors({ email: emailErr, password: passwordErr });
    return valid;
  };


  const handleLogin = async () => {
    if (validate()) {
      const success = await onLogin(email, password);
      if (success)
        navigate('/home');
    }
  };

  return (
    <Container maxWidth="xs">
      <Box mt={8}>
        <Typography variant="h5" gutterBottom textAlign={"center"}>Login</Typography>
        <Stack spacing={2}>
          <TextField
            label="Email"
            type="email"
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
          <Button variant="contained" onClick={handleLogin}>Entrar</Button>
          <Button variant="outlined" onClick={() => navigate('/signUp')}>Cadastrar</Button>
        </Stack>
      </Box>
    </Container>
  )
}