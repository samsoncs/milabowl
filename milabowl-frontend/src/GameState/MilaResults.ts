import { MilaResultsDTO } from "./DTOs/MilaResultDTOs";
import game_state from "./game_state.json";

export function GetMilaResults(): MilaResultsDTO {
  // @ts-expect-error
  return game_state;
}
