import { AxiosError } from "axios";
import type { IAdvanceRequest } from "../../Interfaces/IAdvanceRequest";
import api from "../Api";

export const approveAdvanceRequest = async (advanceRequest: IAdvanceRequest[]): Promise<boolean> => {
  try {
    var response = await api.put('/AdvanceRequest/approve', advanceRequest );

    return true;
  } catch (err) {
    if (err instanceof AxiosError)
      console.error(err.response?.data.message === undefined ? err.response?.data : err.response?.data.message);

    return false;
  }
}