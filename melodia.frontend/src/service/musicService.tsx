const API_BASE_URL = '/api';

export interface MusicViewModel {
    Prompt: string;
    Duracao: number;
    Nome: string;
}

export const generateMusic = async (data: MusicViewModel): Promise<Blob> => {
    try {
        const response = await fetch(`${API_BASE_URL}/Music/gerar`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error(`Erro ao gerar música: ${response.statusText}`);
        }

        const blob = await response.blob();
        return blob;
    } catch (error) {
        console.error('Erro ao chamar API:', error);
        throw error;
    }
};

export const downloadMusic = (blob: Blob, filename: string) => {
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `${filename}.wav`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
};
