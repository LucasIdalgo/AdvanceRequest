import { useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";

import type { CreateContractProps } from "../Props/CreateContractProps"
import { Box, Button, Container, MenuItem, Stack, TextField, Typography } from "@mui/material";
import type { IContract } from "../Interfaces/IContract";
import type { IInstallments } from "../Interfaces/IInstallments";

export const CreateContract: React.FC<CreateContractProps> = ({ createContracts }) => {
  const [totalAmount, setTotalAmount] = useState('');
  const [installments, setInstallments] = useState(1);
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState({ amount: '' });

  const navigate = useNavigate();

  const perInstallment = useMemo(() => {
    const total = parseFloat(totalAmount.replace(',', '.')) || 0;
    return (total / installments).toFixed(2);
  }, [totalAmount, installments]);

  const validate = () => {
    let valid = true;
    let amountError = '';

    const amount = parseFloat(totalAmount.replace(',', '.'));
    if (!totalAmount.trim()) {
      amountError = 'Informe o valor total';
      valid = false;
    } else if (isNaN(amount) || amount <= 0) {
      amountError = 'Valor deve ser maior que zero';
      valid = false;
    }

    setErrors({ amount: amountError });
    return valid;
  };

  const handleContract = async () => {
    if (!validate()) return;
    try {
      const clientId = localStorage.getItem('clientId');
      const clientName = localStorage.getItem('clientName');

      var installment: IInstallments[] = [];
      let date = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate() + 30)
      for (let index = 0; index < installments; index++) {
        const element: IInstallments = {
          InstallmentId: 0,
          Amount: parseFloat(perInstallment.replace(',', '.')),
          ContractId: 0,
          DueDate: date,
          Status: "open",
          Antecipated: false
        };
        date = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 30)
        installment.push(element);
      }

      var contract: IContract = {
        ContractId: 0,
        ClientId: clientId != null ? parseInt(clientId) : 0,
        ClientName: clientName != null ? clientName : "",
        Active: true,
        Installments: installment
      }

      setLoading(true);

      const success = await createContracts(contract);
      if (success)
        navigate('/contract');

    } catch (err) {
      console.error("Erro ao cadastrar contrato: ", err)
    } finally { setLoading(false); }
  }

  return (
    <Container maxWidth="sm">
      <Box mt={8}>
        <Typography variant="h5" gutterBottom>Cadastrar Contrato</Typography>
        <Stack spacing={2}>
          <TextField
            label="Valor Total"
            type="number"
            value={totalAmount}
            onChange={(e) => setTotalAmount(e.target.value)}
            error={!!errors.amount}
            helperText={errors.amount}
            fullWidth
          />

          <TextField
            label="Parcelas"
            select
            value={installments}
            onChange={(e) => setInstallments(Number(e.target.value))}
            fullWidth
          >
            {Array.from({ length: 12 }, (_, i) => i + 1).map((option) => (
              <MenuItem key={option} value={option}>{option}x</MenuItem>
            ))}
          </TextField>

          <Typography variant="subtitle1">
            Valor por parcela: R$ {perInstallment}
          </Typography>

          <Button variant="contained" onClick={handleContract} disabled={loading}>
            {loading ? 'Salvando...' : 'Cadastrar Contrato'}
          </Button>
          <Button variant="outlined" onClick={() => navigate('/contract')}>
            Voltar
          </Button>
        </Stack>
      </Box>
    </Container>
  )
}
