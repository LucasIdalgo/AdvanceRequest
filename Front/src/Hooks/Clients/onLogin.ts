import { AxiosError } from "axios";
import type { IResponseDefault } from "../../Interfaces/Responses/IResponseDefault";
import type { IClientTokenDTO } from "../../Interfaces/Responses/IClientTokenDTO";
import api from "../Api";

export const onLogin = async (email: string, password: string): Promise<boolean> => {
  try {
    var response = await api.get<IResponseDefault<IClientTokenDTO>>(`/Client/login?Email=${email}&Password=${password}`);

    localStorage.setItem('token', response.data.data.token);
    localStorage.setItem('clientName', response.data.data.name);
    localStorage.setItem('clientId', response.data.data.clientId);

    return true;
  } catch (err) {
    if (err instanceof AxiosError)
      console.error(err.response?.data.message === undefined ? err.response?.data : err.response?.data.message);

    return false;
  }
}