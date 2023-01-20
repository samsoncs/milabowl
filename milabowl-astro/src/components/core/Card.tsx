export interface Props {
    title?: string;
    secondary?: React.ReactNode;
    children?: React.ReactNode;
}

const Card2: React.FC<Props> = ({children, title, secondary}) => (
    <div className="bg-white rounded-md p-4 shadow-sm space-y-2 overflow-x-auto">
        <div className="flex">
            <div className="text-xl font-bold text-indigo-900 mb-3 grow ">
                {title}
            </div>
            {secondary}
        </div>
        
        <div className="text-base">
            {children}
        </div>
    </div>
);

export default Card2;