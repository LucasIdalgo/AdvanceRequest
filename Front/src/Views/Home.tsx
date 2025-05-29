import { Box, Button, Container, Stack, Typography } from "@mui/material"
import { useNavigate } from "react-router-dom"

export const Home = () => {
  const navigate = useNavigate();

  const signOut = () => {
    localStorage.clear();
    navigate('/login');
  };

  return (
    <Container maxWidth="xs">
      <Box mt={8}>
        <Typography variant="h4" gutterBottom textAlign={"center"}>Bem vindo</Typography>
        <Typography variant="h4" gutterBottom textAlign={"center"}>{localStorage.getItem('clientName')}</Typography>

        <Stack gap={5} marginTop={10}>
          <Button variant="contained" onClick={() => navigate('/contract')}>Contratos</Button>
          <Button variant="contained" onClick={() => navigate('/advanceRequest')}>Adiantamentos</Button>
          <Button variant="outlined" onClick={signOut}>Sair</Button>
        </Stack>
      </Box>
    </Container>
  )
}