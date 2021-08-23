import axios from 'axios';
import { MilaResultsDTO } from './Dtos/ApiDtos';

export async function GetMilaResults(): Promise<MilaResultsDTO> {
  const response = await axios.get<MilaResultsDTO>('https://samsoncs.github.io/milabowl/game_state.json');
  return response.data;
}