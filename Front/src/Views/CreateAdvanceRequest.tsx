import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Box, Button, Container, MenuItem, Stack, TextField, Typography } from "@mui/material";

import type { IContract } from "../Interfaces/IContract";
import type { CreateAdvanceRequestProps } from "../Props/CreateAdvanceRequestProps";
import { loadContracts } from "../Hooks/Contracts/loadContracts";
import type { IAdvanceRequest } from "../Interfaces/IAdvanceRequest";

export const CreateAdvanceRequest: React.FC<CreateAdvanceRequestProps> = ({ createAdvanceRequest }) => {
  const [contracts, setContracts] = useState<IContract[]>([]);
  const [selectedContractId, setSelectedContractId] = useState(0);
  const [installmentsToAdvance, setInstallmentsToAdvance] = useState(1);
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<{ contract?: string; quantity?: string }>({});

  const navigate = useNavigate();

  const fetchContracts = async () => {
    try {
      var allContracts = await loadContracts(1, 100);

      const eligibleContracts = allContracts.map((contract: IContract) => {
        const today = new Date();
        const filteredInstallments = (contract.installments || []).filter((i) => {
          const dueDate = new Date(i.dueDate);
          const diffDays = (dueDate.getTime() - today.getTime()) / (1000 * 60 * 60 * 24);
          return diffDays > 30 && !i.antecipated;
        });
        return { ...contract, installments: filteredInstallments };
      }).filter((c) => c.installments.length > 0);
      setContracts(eligibleContracts);
    } catch (error) {
      console.error('Erro ao buscar contratos:', error);
    }
  };

  useEffect(() => {
    fetchContracts();
  }, []);

  const handleAdvance = async () => {
    const newErrors: { contract?: string; quantity?: string } = {};

    if (!selectedContractId)
      newErrors.contract = 'Selecione um contrato';
    if (installmentsToAdvance < 1)
      newErrors.quantity = 'Informe uma quantidade válida';

    const selectedContract = contracts.find(c => c.contractId === selectedContractId);
    const maxAdvance = selectedContract?.installments.length || 0;

    if (installmentsToAdvance > maxAdvance)
      newErrors.quantity = `Este contrato permite no máximo ${maxAdvance} antecipações.`;

    setErrors(newErrors);
    if (Object.keys(newErrors).length > 0) return;

    try {
      const clientId = localStorage.getItem('clientId');

      var advanceRequest: IAdvanceRequest = {
        AdvanceRequestId: 0,
        ClientId: clientId != null ? parseInt(clientId) : 0,
        ContractId: selectedContractId,
        InstallmentQuantity: installmentsToAdvance,
        Approve: false,
        CreatedAt: new Date(),
        ApprovetAt: null
      };

      setLoading(true);
      const success = await createAdvanceRequest(advanceRequest);
      if (success)
        navigate('/advanceRequest');

    } catch (error) {
      console.error('Erro ao solicitar antecipação:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="sm">
      <Box mt={4}>
        <Typography variant="h5" gutterBottom>Solicitar Antecipação</Typography>
        <Stack spacing={2}>
          <TextField
            label="Contrato"
            select
            value={selectedContractId}
            onChange={(e) => setSelectedContractId(Number(e.target.value))}
            fullWidth
            error={!!errors.contract}
            helperText={errors.contract}
          >
            {contracts.map((contract) => (
              <MenuItem key={contract.contractId} value={contract.contractId}>
                {contract.contractId} - {contract.clientName} ({contract.installments.length} parcelas disponíveis)
              </MenuItem>
            ))}
          </TextField>

          <TextField
            label="Quantidade de parcelas a antecipar"
            type="number"
            value={installmentsToAdvance}
            onChange={(e) => setInstallmentsToAdvance(Number(e.target.value))}
            fullWidth
            error={!!errors.quantity}
            helperText={errors.quantity}
          />

          <Button variant="contained" onClick={handleAdvance} disabled={loading}>
            {loading ? 'Solicitando...' : 'Solicitar Antecipação'}
          </Button>

          <Button variant="outlined" onClick={() => navigate('/advanceRequest')}>
            Voltar
          </Button>
        </Stack>
      </Box>
    </Container>
  )
}
