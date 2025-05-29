import type { IAdvanceRequest } from "../Interfaces/IAdvanceRequest";

export interface AdvanceRequestProps {
  loadAdvanceRequest: (page: number, limit: number) => Promise<IAdvanceRequest[]>;
}