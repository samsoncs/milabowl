export interface Props {
    title?: string;
    secondary?: React.ReactNode;
    children?: React.ReactNode;
}

const Card2: React.FC<Props> = ({children, title, secondary}) => (
    <div className="bg-white dark:bg-slate-800 rounded-md p-4 shadow-sm dark:shadow-sm-dark space-y-2">
        <div className="flex">
            <div className="text-xl font-bold text-indigo-900 dark:text-dark-text mb-3 grow">
                {title}
            </div>
            {secondary}
        </div>
        
        <div className="text-base dark:text-dark-text overflow-x-auto">
            {children}
        </div>
    </div>
);

export default Card2;