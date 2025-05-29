import { AxiosError } from "axios";

import type { IAdvanceRequest } from "../../Interfaces/IAdvanceRequest";

import api from "../Api";

export const createdvanceRequest= async (advanceRequest:IAdvanceRequest): Promise<boolean>=>{
try {
    var response = await api.post('/AdvanceRequest',{
      ContractId:advanceRequest.ContractId,
      InstallmentQuantity:advanceRequest.InstallmentQuantity,
      ClientId:advanceRequest.ClientId,
      Approve:advanceRequest.Approve,
      CreatedAt:advanceRequest.CreatedAt
    });

    return true;
  } catch (err) {
    if (err instanceof AxiosError)
      console.error(err.response?.data.message === undefined ? err.response?.data : err.response?.data.message);

    return false;
  }
}