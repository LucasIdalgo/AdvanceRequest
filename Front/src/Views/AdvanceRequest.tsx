import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { DataGrid, type GridColDef, type GridPaginationModel, type GridRowSelectionModel } from "@mui/x-data-grid";
import { Box, Container, Typography, Button } from "@mui/material";

import type { AdvanceRequestProps } from "../Props/AdvanceRequestProps";

import type { IAdvanceRequest } from "../Interfaces/IAdvanceRequest";
import { approveAdvanceRequest } from "../Hooks/AdvanceRequests/approveAdvanceRequest";

export const AdvanceRequest: React.FC<AdvanceRequestProps> = ({ loadAdvanceRequest }) => {
  const [contracts, setContracts] = useState<IAdvanceRequest[]>([]);
  const [pageSize, setPageSize] = useState(5);
  const [page, setPage] = useState(1);
  const [rowCount, setRowCount] = useState(1);
  const [selectedIds, setSelectedIds] = useState<GridRowSelectionModel>();
  const [loading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');

  const navigate = useNavigate();

  const fetchAdvanceRequests = async (page: number, limit: number) => {
    try {
      const items = await loadAdvanceRequest(page, limit);
      setContracts(items);
      setRowCount(items.length);
    } catch (error) {
      console.error('Erro ao buscar as antecipações:', error);
    }
  };

  useEffect(() => {
    fetchAdvanceRequests(page, pageSize);
  }, [page, pageSize]);

  const columns: GridColDef[] = [
    { field: 'advanceRequestId', headerName: 'ID', width: 90 },
    { field: 'clientId', headerName: 'Cliente ID', width: 120 },
    { field: 'contractId', headerName: 'Contrato ID', width: 120 },
    { field: 'approve', headerName: 'Aprovado', width: 100, type: 'boolean' },
    {
      field: 'approvedAt', headerName: 'Data Aprovação', width: 130, type: 'date', renderCell: (params) => {
        const date = params.row.approvedAt == null || params.row.approvedAt == undefined ? null : new Date(params.row.approvedAt);
        return date ? date.toLocaleDateString('pt-BR') : 'N/A';
      }
    },
    {
      field: 'createdAt', headerName: 'Data Solicitação', width: 140, renderCell: (params) => {
        const date = params.row.createdAt == null || params.row.createdAt == undefined ? null : new Date(params.row.createdAt);
        return date ? date.toLocaleDateString('pt-BR') : 'N/A';
      }
    }
  ];

  const handleApprove = async () => {
    try {
      setLoading(true);
      setErrorMessage('');

let advanceRequest: IAdvanceRequest[] = [];

      for (let index = 0; index < selectedIds?.ids.size; index++) {
        const element = contracts[index];
        advanceRequest.push(element)
      };

      const success = await approveAdvanceRequest(advanceRequest);
      if (success) {
        fetchAdvanceRequests(page, pageSize);
        setSelectedIds(undefined);
      }
    } catch (err) {
      console.error('Erro ao aprovar:', err);
      setErrorMessage('Falha ao aprovar antecipações.');
    } finally {
      setLoading(false);
    }
  };

  const handlePaginationChange = (model: GridPaginationModel) => {
    setPage(model.page);
    setPageSize(model.pageSize);
  };

  return (
    <Container maxWidth="lg">
      <Box mt={4} mb={2} display="flex" justifyContent="space-between" alignItems="center">
        <Typography variant="h4">Antecipações</Typography>
        <Box>
          <Button sx={{ mr: 2 }} variant="outlined" onClick={() => navigate('/home')}>Voltar</Button>
          <Button sx={{ mr: 2 }} variant="contained" onClick={handleApprove} disabled={
            selectedIds == null || selectedIds == undefined || selectedIds.length === 0 || loading}>
            Aprovar Selecionadas
          </Button>
          <Button variant="contained" onClick={() => navigate('/advanceRequest/create')}>Nova Antecipação</Button>
        </Box>
      </Box>

      {errorMessage && (
        <Box mb={2}>
          <Typography color="error">{errorMessage}</Typography>
        </Box>
      )}

      <Box height={400}>
        <DataGrid
          rows={contracts}
          columns={columns}
          getRowId={(row) => row.advanceRequestId}
          pageSizeOptions={[pageSize]}
          rowCount={rowCount}
          paginationMode="server"
          onPaginationModelChange={handlePaginationChange}
          paginationModel={{ page, pageSize }}
          checkboxSelection

          onRowSelectionModelChange={(ids) => setSelectedIds(ids)}
          rowSelectionModel={selectedIds}
        />
      </Box>
    </Container>
  )
}