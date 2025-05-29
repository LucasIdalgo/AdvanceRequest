import { AxiosError } from "axios"

import api from "../Api";

import type { IContract } from "../../Interfaces/IContract";

export const loadContracts = async (page: number, limit: number): Promise<IContract[]> => {
  try {
    const clientId = localStorage.getItem('clientId');
    var response = await api.get(`/Contract/byClient?IdClient=${clientId}&Page=${page == 0? 1 : page}&Limit=${limit}`);

    return response.data.data;
  } catch (err) {
    if (err instanceof AxiosError)
      console.error(err.response?.data.message === undefined ? err.response?.data : err.response?.data.message);

    return [];
  }
}