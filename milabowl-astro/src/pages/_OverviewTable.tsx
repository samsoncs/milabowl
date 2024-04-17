import { createColumnHelper, type RowData } from "@tanstack/react-table";
import type { GameWeekResult } from "../game_state/gameState";
import SortableTable from "../components/core/Table/SortableTable";
import {useMemo} from "react";
import PositionDelta from "../components/core/PositionDelta";
const images = import.meta.glob<{ default: ImageMetadata }>('/src/assets/*.{webp}')
import ac_mila from "../assets/ac_mila.webp";
import '@tanstack/react-table'

declare module '@tanstack/react-table' {
  interface ColumnMeta<TData extends RowData, TValue> {
    align?: "right" | "left";
    padding?: string;
  }
}

const columnHelper = createColumnHelper<GameWeekResult>();

interface Props{
    data: GameWeekResult[];
    lastGameWeek: number;
}

const OverviewTable: React.FC<Props> = ({data, lastGameWeek}) => {
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
            )}
        }),
        columnHelper.accessor("teamName", {
            id: "teamName",
            header: "Team",
            cell: (props) => (
            <span className="flex items-center gap-2 max-w-10">
                <img src={ac_mila.src} className="h-9 w-9 rounded-full"/>
                <a className="underline max-w-[120px] truncate" href={`/fpl/players/${props.row.original.teamName.replaceAll(" ", "-")}/gw/${lastGameWeek}`}>
                    {props.cell.getValue()}
                </a>
            </span>
            ),
            enableSorting: false
        }),
        columnHelper.accessor("milaPoints.total", {
            id: "gameWeek",
            header: "GW",
            meta: {
                align: "right"
            }
        }),
        // columnHelper.accessor("cumulativeAverageMilaPoints", {
        //     id: "avg",
        //     header: (props) => (<span className="hidden md:table-cell">Avg</span>),
        //     cell: (props) => (
        //         <span className="hidden md:table-cell">{props.getValue()}</span>
        //     ),
        //     meta: {
        //         align: "right"
        //     }
        // }),
        columnHelper.accessor("cumulativeMilaPoints", {
            id: "total",
            header: "Total",
            cell: (props) => (<span className="font-bold text-indigo-900 dark:text-orange-200">{props.getValue()}</span>),
            meta: {
                align: "right",
                padding: "pl-0 md:pl-3"
            }
        })
      ],[]);
          
    return (
    <SortableTable data={data} columns={columns}/>
    )
};

export default OverviewTable;