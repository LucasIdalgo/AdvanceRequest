import { AxiosError } from "axios";
import api from "../Api";
import type { IResponseDefault } from "../../Interfaces/Responses/IResponseDefault";
import type { IClientDTO } from "../../Interfaces/Responses/IClientDTO";

export const onRegister = async (name: string, email: string, password: string) => {
  try {
    var response = await api.post<IResponseDefault<IClientDTO>>('/Client', { name, email, password });
    console.log(response.data.message)
    
    return true;
  } catch (err) {
    if (err instanceof AxiosError)
      console.error(err.response?.data.message === undefined ? err.response?.data : err.response?.data.message);

    return false;
  }
};