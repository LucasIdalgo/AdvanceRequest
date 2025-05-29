import { AxiosError } from "axios";
import type { IAdvanceRequest } from "../../Interfaces/IAdvanceRequest";
import api from "../Api";

export const approveAdvanceRequest= async (advanceRequest:IAdvanceRequest[]): Promise<boolean>=>{
try {
  const payload = advanceRequest.map(item=>({
    ContractId:item.contractId,
      InstallmentQuantity:item.istallmentQuantity,
      ClientId:item.clientId,
      Approve:item.approve,
      CreatedAt:item.createdAt,
      AdvanceRequestId:item.advanceRequestId
  }))
    var response = await api.put('/AdvanceRequest/approve',{payload});

    return true;
  } catch (err) {
    if (err instanceof AxiosError)
      console.error(err.response?.data.message === undefined ? err.response?.data : err.response?.data.message);

    return false;
  }
}