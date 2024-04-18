import { createColumnHelper } from "@tanstack/react-table";
import type { GameWeekResult } from "../../game_state/gameState";
import { useMemo } from "react";
import PositionDelta from "../../components/core/PositionDelta";
import SortableTable from "../../components/core/Table/SortableTable";

const columnHelper = createColumnHelper<GameWeekResult>();

interface Props{
    data: GameWeekResult[];
    lastGameWeek: number;
    avatars: ImageMetadata[];
}


const StandingsTable: React.FC<Props> = ({data, lastGameWeek, avatars}) => {
    const columns = useMemo(() => [
        columnHelper.display({
            id: "rank",
            header: "#",
            cell: (props) => { 
                const deltaPosition =
                props.row.original.milaRankLastWeek === null ||
                props.row.original.milaRankLastWeek === undefined
                  ? 0
                  : props.row.original.milaRankLastWeek -  props.row.original.milaRank;

                return(
                <div className="flex flex-col items-center justify-between gap-1">
                    <PositionDelta pos={props.row.original.milaRank} delta={deltaPosition} />
                </div>
            )},
            size: 40
        }),
        columnHelper.accessor("teamName", {
            id: "teamName",
            header: "Team",
            cell: (props) => (
            <span className="flex items-center gap-2">
                <img src={avatars.find(a => a.src.includes(props.row.original.teamName.replace("$", "s").toLowerCase().replaceAll(" ", "_")))?.src} className="h-9 w-9 rounded-full"/>
                <a className="underline max-w-[130px] sm:max-w-[300px] truncate" href={`/fpl/players/${props.row.original.teamName.replaceAll(" ", "-")}/gw/${lastGameWeek}`}>
                    {props.cell.getValue()}
                </a>
            </span>
            ),
            enableSorting: false,
            size: 230,
            maxSize: 400
        }),
        columnHelper.accessor("milaPoints.gW69",{
            id: "69gw",
            header: "69"
        }),
        columnHelper.accessor("milaPoints.yellowCard",{
            id: "yc",
            header: "YC"
        }),
        columnHelper.accessor("milaPoints.redCard",{
            id: "rc",
            header: "RC"
        }),
        columnHelper.accessor("milaPoints.benchFail",{
            id: "bf",
            header: "BF"
        }),
        columnHelper.accessor("milaPoints.capFail",{
            id: "cf",
            header: "CF"
        }),
        columnHelper.accessor("milaPoints.capDef",{
            id: "cd",
            header: "CD"
        }),
        columnHelper.accessor("milaPoints.minusIsPlus",{
            id: "mip",
            header: "MiP"
        }),
        columnHelper.accessor("milaPoints.increaseStreak",{
            id: "is",
            header: "IS"
        }),
        columnHelper.accessor("milaPoints.equalStreak",{
            id: "es",
            header: "ES"
        }),
        columnHelper.accessor("milaPoints.gwPositionScore",{
            id: "gwps",
            header: "GW PS"
        }),
        columnHelper.accessor("milaPoints.headToHeadMeta",{
            id: "h2hm",
            header: "H2H M"
        }),
        columnHelper.accessor("milaPoints.sixtyNineSub",{
            id: "69sub",
            header: "69 Sub"
        }),
        columnHelper.accessor("milaPoints.uniqueCap",{
            id: "unqCap",
            header: "Unq Cap"
        }),
        columnHelper.accessor("milaPoints.trendyBitch",{
            id: "trnd",
            header: "Trnd"
        }),
        columnHelper.accessor("milaPoints.penaltiesMissed",{
            id: "pn",
            header: "Pn"
        }),
        columnHelper.accessor("milaPoints.sellout",{
            id: "$o",
            header: "$O"
        }),
        columnHelper.accessor("milaPoints.activeChip",{
            id: "chp",
            header: "Chip"
        }),
        columnHelper.accessor("milaPoints.greenShell",{
            id: "gsh",
            header: "GS"
        }),
        columnHelper.accessor("milaPoints.redShell",{
            id: "rsh",
            header: "RS"
        }),
        columnHelper.accessor("milaPoints.mushroom",{
            id: "mshr",
            header: "Mshr"
        }),
        columnHelper.accessor("milaPoints.banana",{
            id: "bn",
            header: "Bn"
        }),
        columnHelper.accessor("milaPoints.bombPoints",{
            id: "bmb",
            header: "Bmb"
        }),
        columnHelper.accessor("milaPoints.darthMaulPoints",{
            id: "drth",
            header: "Drth"
        }),
        columnHelper.accessor("milaPoints.total",{
            id: "gwmp",
            header: "GW MP"
        }),
        columnHelper.accessor("cumulativeMilaPoints",{
            id: "totmp",
            header: "Total MP"
        }),
    ], []);

    return(
        <SortableTable data={data} columns={columns} initialColumnPinnings={["rank", "teamName"]}/>
    )
};

export default StandingsTable;