export interface Props {
  title?: string;
  secondary?: React.ReactNode;
  children?: React.ReactNode;
}

const Card2: React.FC<Props> = ({ children, title, secondary }) => (
  <div className="bg-surface border-1 border-outline/30 h-full space-y-2 rounded-md p-4">
    <div className="flex">
      <div className="text-content-primary mb-3 grow font-bold">{title}</div>
      {secondary}
    </div>

    <div className="overflow-x-auto text-base">{children}</div>
  </div>
);

export default Card2;
