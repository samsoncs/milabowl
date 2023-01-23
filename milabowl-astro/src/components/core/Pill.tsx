export interface PillProps {
    title: string;
    color: string;
    disabled?: boolean;
}

const Pill: React.FC<PillProps> = ({title, color, disabled}) => (
    <div className={ ["rounded-full", "text-white", "text-xs md:text-sm", "md:text-base", "text-center", "px-2", color, disabled ? "opacity-50" : ""].join(" ") }>
        {title}
    </div>
)

export default Pill;