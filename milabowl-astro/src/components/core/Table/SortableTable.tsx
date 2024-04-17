import { useReactTable,getCoreRowModel, flexRender, type SortingState, getSortedRowModel, type ColumnDef } from '@tanstack/react-table'
import { useState } from 'react';

interface Props<T>{
    data: T[];
    columns: ColumnDef<T, any>[];
}

const SortableTable = <T,>({ data, columns }: Props<T>) => {
    const [sorting, setSorting] = useState<SortingState>([])

    const table = useReactTable({
        data,
        columns,
        getCoreRowModel: getCoreRowModel(),
        getSortedRowModel: getSortedRowModel(),
        onSortingChange: setSorting,
        state:{
            sorting
        }
    });
   

    return(
        <table className="h-full w-full">
            <thead>
            {
                table.getHeaderGroups().map(hg => (
                    <tr key={hg.id}>
                    {
                        hg.headers.map(header => (
                            <th className={`p-3 text-sm p-2 ${header.column.columnDef.meta?.padding} ${header.column.getIsFirstColumn() ? "pl-0" : ""} ${header.column.getIsLastColumn() ? "pr-0" : ""} ${header.column.columnDef.meta?.align === "right" ? "text-right" : "text-left"}`} key={header.id}>
                                <div className="flex gap-0.5" onClick={header.column.getToggleSortingHandler()}>
                                {flexRender(
                                    header.column.columnDef.header, 
                                    header.getContext()
                                )}
                                {{
                                    asc: <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" className="w-5 h-5">
                                    <path stroke-linecap="round" stroke-linejoin="round" d="M3 4.5h14.25M3 9h9.75M3 13.5h5.25m5.25-.75L17.25 9m0 0L21 12.75M17.25 9v12" />
                                  </svg>,
                                    desc: <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" className="w-5 h-5">
                                    <path stroke-linecap="round" stroke-linejoin="round" d="M3 4.5h14.25M3 9h9.75M3 13.5h9.75m4.5-4.5v12m0 0-3.75-3.75M17.25 21 21 17.25" />
                                  </svg>,
                                }[header.column.getIsSorted() as string] ?? null}
                                {
                                    !header.column.getIsSorted() && header.column.getCanSort() && (
                                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" className="w-5 h-5 dark:text-slate-500">
                                            <path stroke-linecap="round" stroke-linejoin="round" d="M3 7.5 7.5 3m0 0L12 7.5M7.5 3v13.5m13.5 0L16.5 21m0 0L12 16.5m4.5 4.5V7.5" />
                                        </svg>                                      
                                    )
                                }
                                </div>
                            </th>
                        ))   
                    }
                    </tr>
                ))
            }
            </thead>
            <tbody className="w-full">
            {
                table.getRowModel().rows.map(row => (
                    <tr className={`border-b border-slate-200 dark:border-slate-700 text-sm`} key={row.id}>
                        {
                            row.getVisibleCells().map(cell => (
                                <td className={`p-2 align-middle text-center ${cell.column.columnDef.meta?.padding} ${cell.column.getIsFirstColumn() ? "pl-0" : ""} ${cell.column.getIsLastColumn() ? "pr-0" : ""} ${cell.column.columnDef.meta?.align === "right" ? "text-right" : ""}`} key={cell.id}>
                                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                                </td>
                            ))
                        }
                    </tr>
                ))
            }
            </tbody>
        </table>
    );
};

interface CustomHeaderProps{
    children?: React.ReactNode;
}

export const CustomHeader: React.FC<CustomHeaderProps> = ({children}) => (
    <div>{children}</div>
);

export default SortableTable;