import { Box, Button, Container, Typography } from "@mui/material"
import { DataGrid, type GridColDef, type GridPaginationModel } from '@mui/x-data-grid';

import type React from "react";
import { useNavigate } from "react-router-dom"
import { useEffect, useState } from "react";

import type { ContractProps } from "../Props/ContractProps";

import type { IContract } from "../Interfaces/IContract";
import type { IInstallments } from "../Interfaces/IInstallments";

export const Contract: React.FC<ContractProps> = ({ loadContracts }) => {
  const [contracts, setContracts] = useState<IContract[]>([]);
  const [pageSize, setPageSize] = useState(5);
  const [page, setPage] = useState(1);
  const [rowCount, setRowCount] = useState(1);

  const navigate = useNavigate();

  const fetchContracts = async (page: number, limit: number) => {
    try {
      const items = await loadContracts(page, limit);
      setContracts(items);
      setRowCount(items.length);
    } catch (error) {
      console.error('Erro ao buscar contratos:', error);
    }
  };

  useEffect(() => {
    fetchContracts(page, pageSize);
  }, [page, pageSize]);

  const columns: GridColDef[] = [
    { field: 'contractId', headerName: 'ID', width: 90 },
    { field: 'clientId', headerName: 'Cliente ID', width: 120 },
    { field: 'clientName', headerName: 'Nome do Cliente', width: 200 },
    {
      field: 'installmentsSummary',
      headerName: 'Parcelas',
      width: 500,
      renderCell: (params) => {
        const installments = params.row.installments as IInstallments[];
        if (!installments || installments.length === 0) return 'Nenhuma';
        return installments.map(i => `R$ ${i.amount.toFixed(2)} - ${new Date(i.dueDate).toLocaleDateString()}`).join(' | ');
      }
    },
    { field: 'active', headerName: 'Ativo', width: 100, type: 'boolean' }
  ];

  const handlePaginationChange = (model: GridPaginationModel) => {
    setPage(model.page);
    setPageSize(model.pageSize);
  };

  return (
    <Container maxWidth="lg">
      <Box mt={4} mb={2} display="flex" justifyContent="space-between" alignItems="center">
        <Typography variant="h4">Contratos</Typography>
        <Box>
          <Button sx={{mr: 2}} variant="outlined" onClick={() => navigate('/home')}>Voltar</Button>
          <Button variant="contained" onClick={() => navigate('/contract/create')}>Novo Contrato</Button>
        </Box>
      </Box>
      <Box height={400}>
        <DataGrid
          rows={contracts}
          columns={columns}
          getRowId={(row) => row.contractId}
          pageSizeOptions={[pageSize]}
          rowCount={rowCount}
          paginationMode="server"
          onPaginationModelChange={handlePaginationChange}
          paginationModel={{ page, pageSize }}
          checkboxSelection
          disableRowSelectionOnClick
        />
      </Box>
    </Container>
  )
}