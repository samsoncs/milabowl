import { MilaResultsDTO } from './Dtos/ApiDtos';
import game_state from '../game_state.json';

export async function GetMilaResults(): Promise<MilaResultsDTO> {
  //@ts-ignore
  return game_state;
}