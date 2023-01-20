import { useState } from "react";
import Card from "../../components/core/Card";
import Pill from "../../components/core/Pill";
import type { Nomination } from "../../game_state/nominations";

export interface NominationsProps {
    nominations: Nomination[];
}

const GetChipColor = (
category: string
):string => {
    switch (category) {
        case "SKILL":
        return "bg-sky-600";
        case "SHAME":
        return "bg-red-600";
        case "DERP":
        return "bg-yellow-500";
        case "LUCK":
        return "bg-green-600";
        default:
        return "bg-gray-600";
    }
};
  
const GetIcon = (category: string): string => {
    switch (category) {
        case "SKILL":
        return "⭐";
        case "SHAME":
        return "🤦";
        case "DERP":
        return "🥴";
        case "LUCK":
        return "🍀";
        default:
        return "";
    }
};

interface NominationChipProps {
    category: "SKILL" | "SHAME" | "DERP" | "LUCK";
    filters: string[];
    setFilter: (value: React.SetStateAction<string[]>) => void;
}

const NominationChip: React.FC<NominationChipProps> = ({ category, filters, setFilter }) => (
    <div onClick={() => {
        setFilter((of) => {
            if (of.includes(category)) {
            return of.filter((item) => item !== category);
            }

            if (of.length > 2) {
            return [];
            }

            return [...of, category];
        });
    }}>
        <Pill 
            title={`${GetIcon(category)}${category}`} 
            color={`${GetChipColor(category)}`}
            disabled={filters.length > 0 && !filters.includes(category)}
        />
    </div>
);

const Nominations: React.FC<NominationsProps> = ({nominations}) => {
    const [filters, setFilters] = useState<string[]>([]);

    return(
    <div className="max-w-screen-lg w-screen mx-auto flex flex-col space-y-4"> 
        <div className="flex space-x-2">
            <div className="font-bold">
                Filter:
            </div>
            <NominationChip category="SKILL" filters={filters} setFilter={setFilters}/>
            <NominationChip category="SHAME" filters={filters} setFilter={setFilters}/>
            <NominationChip category="DERP" filters={filters} setFilter={setFilters}/>
            <NominationChip category="LUCK" filters={filters} setFilter={setFilters}/>
        </div>
      
        {          
            nominations.filter((f) => filters.length === 0 || filters.includes(f.category)).map(n => (

                <Card
                    key={`GW ${n.gw} - ${n.personNominated} - ${n.reason}`} 
                    title={`GW ${n.gw} - ${n.personNominated}`} 
                    secondary={
                        <div className="flex items-center space-x-4" slot="secondary">
                            <Pill title={`${GetIcon(n.category)}${n.category}`} color={GetChipColor(n.category)}/>
                            <div className="text-sm text-neutral-500">
                                {n.date}
                            </div>
                        </div>
                    }>
                
                    {n.reason}
                </Card>
            ))
        }
    </div>
)}

export default Nominations;