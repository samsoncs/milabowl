export interface Props {
  title?: string;
  secondary?: React.ReactNode;
  children?: React.ReactNode;
}

const Card2: React.FC<Props> = ({ children, title, secondary }) => (
  <div className="bg-surface border-outline space-y-2 rounded-md border-t p-4 shadow-sm dark:shadow-sm-dark">
    <div className="flex">
      <div className="text-content-primary mb-3 grow font-bold">{title}</div>
      {secondary}
    </div>

    <div className="overflow-x-auto text-base">{children}</div>
  </div>
);

export default Card2;
