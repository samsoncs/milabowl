import axios from 'axios';
import { MilaResultsDTO } from './Dtos/ApiDtos';
import { getAccessTokenFromCache } from './AuthService';

const BASE_API_URI: string = process.env.REACT_APP_BASE_API_URI as string;

export async function GetMilaResults(): Promise<MilaResultsDTO> {
  const token = await getAccessTokenFromCache();

  const response = await axios.get<MilaResultsDTO>(`${BASE_API_URI}/api/milaresults`,
    {
      headers: { Authorization: `Bearer ${token}` },
    });
  return response.data;
}

export async function ImportData(): Promise<number> {
  const token = await getAccessTokenFromCache();

  const response = await axios.get<MilaResultsDTO>(`${BASE_API_URI}/api/dataimport`,
    {
      headers: { Authorization: `Bearer ${token}` },
    });
  return response.status;
}

export async function ProcessData(): Promise<number> {
  const token = await getAccessTokenFromCache();

  const response = await axios.get<MilaResultsDTO>(`${BASE_API_URI}/api/dataimport/process`,
    {
      headers: { Authorization: `Bearer ${token}` },
    });
  return response.status;
}

export const GetProfilePicture = async (): Promise<string> => {
  const token = await getAccessTokenFromCache();

  console.log(token);

  const response = await axios.get<string>(`https://graph.microsoft.com/v1.0/me/photo`,
    {
      headers: { Authorization: `Bearer ${token}` },
    });
  return response.data;
}
  