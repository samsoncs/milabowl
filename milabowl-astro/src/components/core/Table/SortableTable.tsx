import { useState } from "react";

interface Props{

}

type Test = {
    name: string;
    age: number;
    points: number;
}

const SortableTable: React.FC<Props> = ({}) => {

    const [sortState, setSortState] = useState<{col: keyof Test, sortOrder: "asc" | "desc"}>({ col: "age", sortOrder: "asc" });

    const [data, setData] = useState<Test[]>([
        { name: "Hans", age: 69, points: 245 },
        { name: "Grete", age: 18, points: 42 },
        { name: "Grete", age: 18, points: 1337 },
        { name: "Ahmed", age: 18, points: 214 },
        { name: "Bruce", age: 18, points: 93 },
    ]);

    const columns: ({id: keyof Test, header: string, sortable?: boolean})[] = [
        { id: "name", header: "Name" },
        { id: "age", header: "Age", sortable: true },
        { id: "points", header: "Points", sortable: true },
    ];

    const sort = (col: keyof Test) => {
        const sortOrder = col !== sortState.col ? "asc" : (sortState.sortOrder === "asc" ? "desc" : "asc");

        const sortedData = [...data].sort((a, b) => {
            const valA = a[col].valueOf();
            const valB = b[col].valueOf();
            const sortMultiplier = sortOrder === "desc" ? 1 : -1;

            if(!isNaN(Number(valA)) && !isNaN(Number(valB))){
                return (Number(valA) - Number(valB)) * sortMultiplier;
            }   

            return valA.toString().toLocaleLowerCase().localeCompare(valB.toString().toLocaleLowerCase()) * sortMultiplier;
        });

        setSortState({col, sortOrder});  
        setData(
            sortedData
        );
    };

    return(
        <table>
            <thead>
            {
            <tr>
            {
                columns.map(c => (
                    <th onClick={() => c.sortable ? sort(c.id) : undefined} key={c.id}>
                        <div className="flex items-center text-sm p-2">
                        {c.header}
                        {
                            c.id === sortState.col && c.sortable ? 
                            <span>
                            {
                                sortState.sortOrder === "asc" ? 
                                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" className="w-4 h-4">
                                    <path stroke-linecap="round" stroke-linejoin="round" d="m4.5 15.75 7.5-7.5 7.5 7.5" />
                                </svg>
                                : 
                                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" className="w-4 h-4">
                                    <path stroke-linecap="round" stroke-linejoin="round" d="m19.5 8.25-7.5 7.5-7.5-7.5" />
                                </svg>
                            }
                            </span> 
                            : 
                            undefined
                        }
                          {
                            c.id !== sortState.col && c.sortable ? 
                            <span className="w-4"/>
                            : 
                            undefined
                        }
                        </div>
                    </th>
                ))
            }
            </tr>
            }

            </thead>
            <tbody>
            {   
                data.map((d,i) => (
                    <tr className="border-b border-slate-200 dark:border-slate-500" key={`tr-${i}`}>
                    {
                        Object.values(d).map(v => (
                            <td className="p-2" key={`td-${i}-${v}`}>{v}</td>
                        ))
                    }
                    </tr>
                ))
            }
            </tbody>
        </table>
    );
};

export default SortableTable;