import {
    useReactTable,
    getCoreRowModel,
    flexRender,
    type SortingState,
    getSortedRowModel,
    type ColumnDef,
    type ColumnPinningState,
    type Column,
} from '@tanstack/react-table';
import { useState, type CSSProperties } from 'react';

interface Props<T> {
    data: T[];
    columns: ColumnDef<T, any>[];
    initialColumnPinnings?: string[];
}

const getCommonPinningStyles = (column: Column<any>): CSSProperties => {
    const isPinned = column.getIsPinned();
    const isLastLeftPinnedColumn =
        isPinned === 'left' && column.getIsLastColumn('left');
    const isFirstRightPinnedColumn =
        isPinned === 'right' && column.getIsFirstColumn('right');

    return {
        boxShadow: isLastLeftPinnedColumn
            ? '-4px 0 4px -4px gray inset'
            : isFirstRightPinnedColumn
              ? '4px 0 4px -4px gray inset'
              : undefined,
        left: isPinned === 'left' ? `${column.getStart('left')}px` : undefined,
        right:
            isPinned === 'right' ? `${column.getAfter('right')}px` : undefined,
        position: isPinned ? 'sticky' : 'relative',
        width: column.getSize(),
        zIndex: isPinned ? 1 : 0,
    };
};

const SortableTable = <T,>({
    data,
    columns,
    initialColumnPinnings,
}: Props<T>) => {
    const [sorting, setSorting] = useState<SortingState>([]);
    const [columnPinning, setColumnPinning] = useState<ColumnPinningState>({
        left: initialColumnPinnings ? [...initialColumnPinnings] : [],
        right: [],
    });

    const table = useReactTable({
        data,
        columns,
        getCoreRowModel: getCoreRowModel(),
        getSortedRowModel: getSortedRowModel(),
        onSortingChange: setSorting,
        onColumnPinningChange: setColumnPinning,
        state: {
            sorting,
            columnPinning,
        },
    });

    return (
        <table
            style={{
                width: initialColumnPinnings ? table.getTotalSize() : undefined,
            }}
            className="w-full"
        >
            <thead>
                {table.getHeaderGroups().map((hg) => (
                    <tr key={hg.id}>
                        {hg.headers.map((header) => (
                            <th
                                style={
                                    initialColumnPinnings
                                        ? {
                                              ...getCommonPinningStyles(
                                                  header.column
                                              ),
                                          }
                                        : undefined
                                }
                                className={`bg-white p-2 p-3 text-sm dark:bg-slate-800 ${header.column.columnDef.meta?.padding} ${header.column.getIsFirstColumn() ? 'pl-0' : ''} ${header.column.getIsLastColumn() ? 'pr-0' : ''} ${header.column.columnDef.meta?.align === 'right' ? 'text-right' : 'text-left'}`}
                                key={header.id}
                            >
                                <div
                                    className="inline-flex gap-0.5"
                                    onClick={header.column.getToggleSortingHandler()}
                                >
                                    {flexRender(
                                        header.column.columnDef.header,
                                        header.getContext()
                                    )}
                                    {{
                                        asc: (
                                            <svg
                                                xmlns="http://www.w3.org/2000/svg"
                                                fill="none"
                                                viewBox="0 0 24 24"
                                                strokeWidth="1.5"
                                                stroke="currentColor"
                                                className="h-5 w-5"
                                            >
                                                <path
                                                    strokeLinecap="round"
                                                    strokeLinejoin="round"
                                                    d="M3 4.5h14.25M3 9h9.75M3 13.5h5.25m5.25-.75L17.25 9m0 0L21 12.75M17.25 9v12"
                                                />
                                            </svg>
                                        ),
                                        desc: (
                                            <svg
                                                xmlns="http://www.w3.org/2000/svg"
                                                fill="none"
                                                viewBox="0 0 24 24"
                                                strokeWidth="1.5"
                                                stroke="currentColor"
                                                className="h-5 w-5"
                                            >
                                                <path
                                                    strokeLinecap="round"
                                                    strokeLinejoin="round"
                                                    d="M3 4.5h14.25M3 9h9.75M3 13.5h9.75m4.5-4.5v12m0 0-3.75-3.75M17.25 21 21 17.25"
                                                />
                                            </svg>
                                        ),
                                    }[header.column.getIsSorted() as string] ??
                                        null}
                                    {!header.column.getIsSorted() &&
                                        header.column.getCanSort() && (
                                            <svg
                                                xmlns="http://www.w3.org/2000/svg"
                                                fill="none"
                                                viewBox="0 0 24 24"
                                                strokeWidth="1.5"
                                                stroke="currentColor"
                                                className="h-5 w-5 text-slate-400 dark:text-slate-500"
                                            >
                                                <path
                                                    strokeLinecap="round"
                                                    strokeLinejoin="round"
                                                    d="M3 7.5 7.5 3m0 0L12 7.5M7.5 3v13.5m13.5 0L16.5 21m0 0L12 16.5m4.5 4.5V7.5"
                                                />
                                            </svg>
                                        )}
                                    {initialColumnPinnings &&
                                        header.column.getIsPinned() && (
                                            <div
                                                onClick={() =>
                                                    header.column.pin(false)
                                                }
                                            >
                                                <svg
                                                    xmlns="http://www.w3.org/2000/svg"
                                                    fill="none"
                                                    viewBox="0 0 24 24"
                                                    strokeWidth="1.5"
                                                    stroke="currentColor"
                                                    className="h-5 w-5 text-slate-400 dark:text-slate-500"
                                                >
                                                    <path
                                                        strokeLinecap="round"
                                                        strokeLinejoin="round"
                                                        d="M13.5 10.5V6.75a4.5 4.5 0 1 1 9 0v3.75M3.75 21.75h10.5a2.25 2.25 0 0 0 2.25-2.25v-6.75a2.25 2.25 0 0 0-2.25-2.25H3.75a2.25 2.25 0 0 0-2.25 2.25v6.75a2.25 2.25 0 0 0 2.25 2.25Z"
                                                    />
                                                </svg>
                                            </div>
                                        )}
                                    {initialColumnPinnings &&
                                        !header.column.getIsPinned() && (
                                            <div
                                                onClick={() =>
                                                    header.column.pin('left')
                                                }
                                            >
                                                <svg
                                                    xmlns="http://www.w3.org/2000/svg"
                                                    fill="none"
                                                    viewBox="0 0 24 24"
                                                    strokeWidth="1.5"
                                                    stroke="currentColor"
                                                    className="h-5 w-5 text-slate-400 dark:text-slate-500"
                                                >
                                                    <path
                                                        strokeLinecap="round"
                                                        strokeLinejoin="round"
                                                        d="M16.5 10.5V6.75a4.5 4.5 0 1 0-9 0v3.75m-.75 11.25h10.5a2.25 2.25 0 0 0 2.25-2.25v-6.75a2.25 2.25 0 0 0-2.25-2.25H6.75a2.25 2.25 0 0 0-2.25 2.25v6.75a2.25 2.25 0 0 0 2.25 2.25Z"
                                                    />
                                                </svg>
                                            </div>
                                        )}
                                </div>
                            </th>
                        ))}
                    </tr>
                ))}
            </thead>
            <tbody className="w-full">
                {table.getRowModel().rows.map((row) => (
                    <tr
                        className={`border-b border-slate-200 text-sm dark:border-slate-700`}
                        key={row.id}
                    >
                        {row.getVisibleCells().map((cell) => (
                            <td
                                style={
                                    initialColumnPinnings
                                        ? {
                                              ...getCommonPinningStyles(
                                                  cell.column
                                              ),
                                          }
                                        : undefined
                                }
                                className={`bg-white p-2 text-center align-middle dark:bg-slate-800 ${cell.column.columnDef.meta?.padding ?? ''} ${cell.column.getIsFirstColumn() ? 'pl-0' : ''} ${cell.column.getIsLastColumn() ? 'pr-0' : ''} ${cell.column.columnDef.meta?.align === 'right' ? 'text-right' : ''}`}
                                key={cell.id}
                            >
                                {flexRender(
                                    cell.column.columnDef.cell,
                                    cell.getContext()
                                )}
                            </td>
                        ))}
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

interface CustomHeaderProps {
    children?: React.ReactNode;
}

export const CustomHeader: React.FC<CustomHeaderProps> = ({ children }) => (
    <div>{children}</div>
);

export default SortableTable;
