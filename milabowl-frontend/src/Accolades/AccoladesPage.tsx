import React from "react";
import Podium from "../Components/Podium";

interface Accolade {
  title: string;
  firstPlace: string;
  secondPlace: string;
  thirdPlace: string;
}

const accolades: Accolade[] = [
  {
    title: "1. Current leader",
    firstPlace: "WeDidn'tStartMaguire",
    secondPlace: "Haalandaise Saus",
    thirdPlace: "Veni Vidi Vici Mila"
  },
  {
    title: "2. Trendiest bitch",
    firstPlace: "Filip Coutinho",
    secondPlace: "eMILA Smith Rowe",
    thirdPlace: "Veni Vidi Vici Mila"
  },
  {
    title: "3. Sadest player (minus is plus)",
    firstPlace: "Haalandaise Saus",
    secondPlace: "WeDidn'tStartMaguire",
    thirdPlace: "Premier Lag"
  },
  {
    title: "4. Most unique player",
    firstPlace: "WeDidn'tStartMaguire",
    secondPlace: "Milaion dollar squad",
    thirdPlace: "Veni Vidi Vici Mila"
  }
];

const AccoladesPage: React.FC<{}> = () => (
  <>
    {accolades.map((a) => (
      <Podium
        key={a.title}
        title={a.title}
        firstPlaceName={a.firstPlace}
        secondPlaceName={a.secondPlace}
        thirdPlaceName={a.thirdPlace}
      ></Podium>
    ))}
  </>
);

export default AccoladesPage;
