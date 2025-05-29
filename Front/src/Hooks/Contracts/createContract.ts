import { AxiosError } from "axios";

import type { IContract } from "../../Interfaces/IContract";

import api from "../Api";

export const createContract = async (contract: IContract): Promise<boolean> => {
  try {
    await api.post(`/Contract`, {
      ClientId: contract.ClientId,
      ClientName: contract.ClientName,
      Active: contract.Active,
      Installments: contract.Installments
    });

    return true;
  } catch (err) {
    if (err instanceof AxiosError)
      console.error(err.response?.data.message === undefined ? err.response?.data : err.response?.data.message);

    return false;
  }
}