import { AxiosError } from "axios";

import type { IAdvanceRequest } from "../../Interfaces/IAdvanceRequest";

import api from "../Api";

export const loadAdvanceRequest= async (page: number, limit: number): Promise<IAdvanceRequest[]>=>{
try {
    var response = await api.get(`/AdvanceRequest?Page=${page == 0? 1 : page}&Limit=${limit}`);

    return response.data.data;
  } catch (err) {
    if (err instanceof AxiosError)
      console.error(err.response?.data.message === undefined ? err.response?.data : err.response?.data.message);

    return [];
  }
}